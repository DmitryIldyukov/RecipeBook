import { useEffect, useRef, useState } from "react";
import MyButton from "../customComponents/myButton/myButton";
import styles from "./editRecipePage.module.scss";
import { useNavigate, useParams } from "react-router-dom";
import { Ingredient, Step, Tag } from "../../types/recipe";
import arrow from "../../assets/arrow.svg";
import RecipeService from "../../services/recipeService";
import { useAppStore } from "../../hooks/useStore";
import { COOK_TIME_OPTIONS, PORTION_COUNT_OPTIONS, ROUTES } from "../../constants/constants";
import downloadIcon from "../../assets/cloud-download.svg";
import closeIcon from "../../assets/close.svg";
import addPrimaryImage from "../../assets/add.svg";
import addImage from "../../assets/add-ptimary.svg";

export const EditRecipePage = () => {
  const fileInputRef = useRef<HTMLInputElement | null>(null);
  const recipeService = new RecipeService();
  const navigate = useNavigate();

  const { recipeId } = useParams();
  const { userId } = useAppStore();

  const [name, setName] = useState<string>("");
  const [description, setDescription] = useState<string>("");
  const [cookTime, setCookTime] = useState<number>(10);
  const [portionCount, setPortionCount] = useState<number>(1);
  const [imageName, setImageName] = useState<string>("");
  const [imageFile, setImageFile] = useState<File | null>(null);

  const [tags, setTags] = useState<Tag[]>([{ id: undefined, name: "Рецепт" }]);
  const [ingredients, setIngredients] = useState<Ingredient[]>([{ id: undefined, title: "", description: "" }]);
  const [steps, setSteps] = useState<Step[]>([{ id: undefined, description: "" }]);

  const getRecipeById = async (recipeId: string) => {
    try {
      const data = await recipeService.getRecipeById(parseInt(recipeId));
      if (userId !== data.authorId) {
        navigate(ROUTES.HOME);
      }
      setName(data.name);
      setDescription(data.description);
      setCookTime(data.cookTime);
      setPortionCount(data.portionCount);
      setTags(data.tags);
      setIngredients(data.ingredients);
      setSteps(data.steps);
      setImageName(data.imageName);
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    if (!userId) {
      navigate(ROUTES.HOME);
    }

    if (recipeId) {
      void getRecipeById(recipeId);
    }
  }, [recipeId]);

  useEffect(() => {
    if (imageName && recipeId) {
      const fetchImage = async () => {
        try {
          const recipeImage = await recipeService.getRecipeImage(parseInt(recipeId));
          const response = await fetch(recipeImage);
          const blob = await response.blob();
          const file = new File([blob], imageName, { type: blob.type });
          setImageFile(file);
        } catch (error) {
          console.error("Ошибка загрузки изображения рецепта:", error);
        }
      };
      void fetchImage();
    }
  }, [imageName]);

  const addIngredient = () => {
    setIngredients([...ingredients, { id: undefined, title: "", description: "" }]);
  };

  const addStep = () => {
    setSteps([...steps, { id: undefined, description: "" }]);
  };

  const removeIngredient = (index: number) => {
    if (ingredients.length > 1) {
      setIngredients(ingredients.filter((_, i) => i !== index));
    }
  };

  const removeStep = (index: number) => {
    if (steps.length > 1) {
      setSteps(steps.filter((_, i) => i !== index));
    }
  };

  const handleUploadClick = () => {
    fileInputRef.current?.click();
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setImageFile(file);
    }
  };

  const handleIngredientChange = (index: number, field: "title" | "description", value: string) => {
    setIngredients((prevIngredients) =>
      prevIngredients.map((ingredient, i) => (i === index ? { ...ingredient, [field]: value } : ingredient)),
    );
  };

  const handleStepChange = (index: number, value: string) => {
    setSteps((prevSteps) => prevSteps.map((step, i) => (i === index ? { ...step, description: value } : step)));
  };

  const onSubmit = (e: React.FormEvent) => {
    void submitForm(e);
    navigate(ROUTES.RECIPES);
  };

  const submitForm = async (e: React.FormEvent) => {
    e.preventDefault();

    const formData = new FormData();

    if (!userId) {
      return;
    }

    formData.append("AuthorId", userId.toString());
    formData.append("Name", name);
    formData.append("Description", description);
    formData.append("CookTime", cookTime.toString());
    formData.append("PortionCount", portionCount.toString());

    if (imageFile) {
      formData.append("ImageFile", imageFile);
      formData.append("ImageName", imageFile.name);
    }

    tags.forEach((tag, index) => {
      formData.append(`Tags[${index.toString()}].Id`, tag.id?.toString() ?? "");
      formData.append(`Tags[${index.toString()}].Name`, tag.name);
    });

    steps.forEach((step, index) => {
      formData.append(`Steps[${index.toString()}].Id`, step.id?.toString() ?? "");
      formData.append(`Steps[${index.toString()}].Description`, step.description);
    });

    ingredients.forEach((ingredient, index) => {
      formData.append(`Ingredients[${index.toString()}].Id`, ingredient.id?.toString() ?? "");
      formData.append(`Ingredients[${index.toString()}].Title`, ingredient.title);
      formData.append(`Ingredients[${index.toString()}].Description`, ingredient.description);
    });

    try {
      const response = recipeId
        ? await recipeService.updateRecipe(parseInt(recipeId), formData)
        : await recipeService.createRecipe(formData);

      if (response.ok) {
        console.log("Рецепт успешно опубликован!");
      } else {
        const errorMessages = response.json();
        console.error("Произошла ошибка при опубликовании рецепта:", errorMessages);
      }
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <form className={styles.container} onSubmit={onSubmit}>
      <button className={styles.backBtn}>
        <img src={arrow} alt="back" />
        Назад
      </button>
      <div className={styles.row}>
        <h3 className={styles.title}>{recipeId ? "Изменить рецепт" : "Добавить новый рецепт"}</h3>
        <MyButton
          isPrimary={true}
          width="278px"
          height="60px"
          isOnSubmit={true}
          onClick={() => {
            console.log("submit");
          }}
        >
          Опубликовать
        </MyButton>
      </div>
      <div className={styles.recipeMainInfoBlock}>
        <div className={styles.card}>
          <div className={styles.uploadInput} onClick={handleUploadClick}>
            {imageFile && <img src={URL.createObjectURL(imageFile)} alt="recipe" className={styles.image} />}
            <input
              type="file"
              accept=".png, .jpg, .jpeg"
              id="recipeImage"
              className={styles.hidden}
              ref={fileInputRef}
              onChange={handleFileChange}
            />
            {!imageFile && (
              <div className={styles.uploadPlaceholder}>
                <img src={downloadIcon} alt="download" className={styles.downloadIcon} />
                <span className={styles.downloadText}>Загрузите фото готового блюда</span>
              </div>
            )}
          </div>
          <div className={styles.recipeInfo}>
            <input
              type="text"
              placeholder="Название рецепта"
              className={styles.inputText}
              value={name}
              onChange={(e) => {
                setName(e.target.value);
              }}
            />
            <textarea
              name=""
              id=""
              placeholder="Краткое описание рецепта (150 символов)"
              className={styles.area}
              value={description}
              onChange={(e) => {
                setDescription(e.target.value);
              }}
            />
            <input type="text" placeholder="Добавить теги" className={styles.inputText} />
            <div className={styles.secInfoBox}>
              <div className={`${styles.row} ${styles.gap10}`}>
                <select
                  value={cookTime}
                  onChange={(e) => {
                    setCookTime(parseInt(e.target.value));
                  }}
                  className={styles.selectInput}
                >
                  <option value="" disabled>
                    Время готовки
                  </option>
                  {COOK_TIME_OPTIONS.map((option) => (
                    <option key={option} value={option}>
                      {option}
                    </option>
                  ))}
                </select>
                <span className={styles.text}>Минут</span>
              </div>
              <div className={`${styles.row} ${styles.gap10}`}>
                <select
                  value={portionCount}
                  onChange={(e) => {
                    setPortionCount(parseInt(e.target.value));
                  }}
                  className={styles.selectInput}
                >
                  <option value="" disabled>
                    Порций в блюде
                  </option>
                  {PORTION_COUNT_OPTIONS.map((option) => (
                    <option key={option} value={option}>
                      {option}
                    </option>
                  ))}
                </select>
                <span className={styles.text}>Персон</span>
              </div>
            </div>
          </div>
        </div>
        <div className={styles.recipeSecondaryInfoBlock}>
          <div className={styles.ingredientBlock}>
            <p className={styles.ingredientBlockTitle}>Ингредиенты</p>
            <ul className={styles.ingredientBlockContent}>
              {ingredients.length > 0 &&
                ingredients.map((ingredient, index) => (
                  <li key={ingredient.id} className={styles.ingredientInfo}>
                    <button
                      type="button"
                      className={styles.closeIconBox}
                      onClick={() => {
                        removeIngredient(index);
                      }}
                    >
                      <img src={closeIcon} alt="close" className={styles.closeIcon} />
                    </button>
                    <div className={styles.ingredientData}>
                      <input
                        placeholder="Заголовок для ингридиентов"
                        className={styles.ingredientTitle}
                        value={ingredient.title}
                        onChange={(e) => {
                          handleIngredientChange(index, "title", e.target.value);
                        }}
                      />
                      <textarea
                        name=""
                        id=""
                        placeholder="Список подуктов для категории"
                        className={styles.ingredientDescription}
                        value={ingredient.description}
                        onChange={(e) => {
                          handleIngredientChange(index, "description", e.target.value);
                        }}
                      />
                    </div>
                  </li>
                ))}
            </ul>
            <MyButton isPrimary={false} onClick={addIngredient} width="380px" height="60px">
              <img src={addImage} alt="add recipe" /> Добавить заголовок
            </MyButton>
          </div>
          <div className={styles.stepBlock}>
            <ul className={styles.stepsBlock}>
              {steps.length > 0 &&
                steps.map((step, index) => (
                  <li key={step.id} className={styles.stepInfo}>
                    <div className={styles.stepTitleBox}>
                      <p className={styles.stepTitle}>Шаг {index + 1}</p>
                      <button
                        type="button"
                        className={styles.closeIconBox}
                        onClick={() => {
                          removeStep(index);
                        }}
                      >
                        <img src={closeIcon} alt="close" className={styles.closeIcon} />
                      </button>
                    </div>
                    <textarea
                      name=""
                      id=""
                      placeholder="Описание шага"
                      className={styles.stepDescription}
                      value={step.description}
                      onChange={(e) => {
                        handleStepChange(index, e.target.value);
                      }}
                    />
                  </li>
                ))}
              <MyButton isPrimary={false} onClick={addStep} width="380px" height="60px">
                <img src={addImage} alt="add recipe" /> Добавить шаг
              </MyButton>
            </ul>
          </div>
        </div>
      </div>
    </form>
  );
};

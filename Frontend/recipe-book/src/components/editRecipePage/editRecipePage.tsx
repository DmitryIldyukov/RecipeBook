import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useAppStore } from "../../hooks/useStore";
import { recipeService } from "../../services/recipeService";
import { ROUTES } from "../../constants/constants";
import { Tag, Ingredient, Step } from "../../types/recipe";
import MyButton from "../customComponents/myButton/myButton";
import { RecipeForm } from "./recipeForm/recipeForm";
import { ImageUploader } from "./imageUploader/imageUploader";
import { IngredientsList } from "./ingredientsList/ingredientsList";
import { StepsList } from "./stepsList/stepsList";
import styles from "./editRecipePage.module.scss";
import { BackBtn } from "../customComponents/backBtn/backBtn";
import toast from "react-hot-toast";
import { handleError } from "../../utils/errorHandler";

export const EditRecipePage = () => {
  const { recipeId } = useParams();
  const { userId } = useAppStore();
  const navigate = useNavigate();

  const [name, setName] = useState<string>("");
  const [description, setDescription] = useState<string>("");
  const [cookTime, setCookTime] = useState<number>(10);
  const [portionCount, setPortionCount] = useState<number>(1);
  const [imageName, setImageName] = useState<string>("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [tags, setTags] = useState<Tag[]>([]);
  const [ingredients, setIngredients] = useState<Ingredient[]>([{ id: undefined, title: "", description: "" }]);
  const [steps, setSteps] = useState<Step[]>([{ id: undefined, description: "" }]);

  const getRecipeById = (recipeId: string) => {
    recipeService
      .getRecipeById(parseInt(recipeId))
      .then((data) => {
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
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  };

  useEffect(() => {
    if (!userId) {
      navigate(ROUTES.HOME);
    }

    if (recipeId) {
      getRecipeById(recipeId);
    }
  }, [recipeId]);

  useEffect(() => {
    if (recipeId) {
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
  }, [recipeId]);

  const onSubmit = (e: React.FormEvent) => {
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
    formData.append("ImageName", imageName);

    if (imageFile) {
      formData.append("ImageFile", imageFile);
    }

    tags.forEach((tag, index) => {
      formData.append(`Tags[${index.toString()}].TagId`, tag.id?.toString() ?? "");
      formData.append(`Tags[${index.toString()}].Name`, tag.name);
    });

    steps.forEach((step, index) => {
      formData.append(`Steps[${index.toString()}].StepId`, step.id?.toString() ?? "");
      formData.append(`Steps[${index.toString()}].Description`, step.description);
    });

    ingredients.forEach((ingredient, index) => {
      formData.append(`Ingredients[${index.toString()}].IngredientId`, ingredient.id?.toString() ?? "");
      formData.append(`Ingredients[${index.toString()}].Title`, ingredient.title);
      formData.append(`Ingredients[${index.toString()}].Description`, ingredient.description);
    });

    (recipeId ? recipeService.updateRecipe(parseInt(recipeId), formData) : recipeService.createRecipe(formData))
      .then(() => {
        toast.success("Рецепт успешно опубликован");
        navigate(ROUTES.RECIPES);
      })
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при сохранении рецепта");
      });
  };

  const handleFormKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === "Enter") {
      e.preventDefault();
    }
  };

  return (
    <form className={styles.container} onSubmit={onSubmit} onKeyDown={handleFormKeyDown}>
      <BackBtn />
      <div className={styles.row}>
        <h3 className={styles.title}>{recipeId ? "Изменить рецепт" : "Добавить новый рецепт"}</h3>
        <MyButton
          isPrimary={true}
          width="278px"
          height="60px"
          isOnSubmit={true}
          onClick={() => {
            console.log();
          }}
        >
          Опубликовать
        </MyButton>
      </div>
      <div className={styles.recipeMainInfoBlock}>
        <div className={styles.card}>
          <ImageUploader imageFile={imageFile} setImageFile={setImageFile} setImageName={setImageName} />
          <RecipeForm
            name={name}
            description={description}
            cookTime={cookTime}
            portionCount={portionCount}
            tags={tags}
            setName={setName}
            setDescription={setDescription}
            setCookTime={setCookTime}
            setPortionCount={setPortionCount}
            setTags={setTags}
          />
        </div>
      </div>
      <div className={styles.recipeSecondaryInfoBlock}>
        <IngredientsList ingredients={ingredients} setIngredients={setIngredients} />
        <StepsList steps={steps} setSteps={setSteps} />
      </div>
    </form>
  );
};

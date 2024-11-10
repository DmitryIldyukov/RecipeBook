import MyButton from "../customComponents/myButton/myButton";
import styles from "./recipesPage.module.scss";
import icon1 from "../../assets/ic-menu.svg";
import icon2 from "../../assets/ic-cook.svg";
import icon3 from "../../assets/ic-chef.svg";
import icon4 from "../../assets/ic-hlop.svg";
import RecipeService from "../../services/recipeService";
import { useEffect, useState } from "react";
import { Recipe } from "../../types/recipe";
import { Page } from "../../types/page";
import { RecipeCard } from "./recipeCard/recipeCard";
import { useLocation } from "react-router";
import { SearchBar } from "../customComponents/mySearchBar/searchBar";
import { RecipesPageHeader } from "./recipesPageHeader/recipesPageHeader";
import { RecipeTagCard } from "./tagsPanel/recipeTagCard";

type RecipesPageProps = {
  searchString: string;
};

export const RecipesPage = () => {
  const recipeService = new RecipeService();

  const defaultPageSize = 4;
  const location = useLocation();

  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [searchString, setSearchString] = useState("");
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    const initialSearchString = (location.state as RecipesPageProps | undefined)?.searchString ?? "";
    setSearchString(initialSearchString);
    void getAllRecipes(initialSearchString, { pageNumber: 1, pageSize: defaultPageSize });
  }, []);

  const getAllRecipes = async (searchQuery: string, currentPage: Page) => {
    setLoading(true);
    try {
      const response = await recipeService.getRecipeList(searchQuery, currentPage);
      setRecipes(response);
    } catch (error) {
      console.error("Ошибка загрузки рецептов:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleLoadMore = () => {
    setPageNumber((prev) => prev + 1);
    void getAllRecipes(searchString, { pageNumber: pageNumber, pageSize: defaultPageSize });
  };

  const handleTagClick = (tag: string) => {
    setSearchString(tag);
    setPageNumber(1);
    void getAllRecipes(tag, { pageNumber: 1, pageSize: defaultPageSize });
  };

  const tagCards = [
    { title: "Простые блюда", icon: icon1 },
    { title: "Детское", icon: icon2 },
    { title: "От шеф-поваров", icon: icon3 },
    { title: "На праздник", icon: icon4 },
  ];

  return (
    <div className={styles.container}>
      <RecipesPageHeader />
      <div className={styles.cardBox}>
        {tagCards.map((tag) => (
          <RecipeTagCard
            key={tag.title}
            title={tag.title}
            icon={tag.icon}
            onClick={() => {
              handleTagClick(tag.title);
            }}
          />
        ))}
      </div>

      <div className={styles.searchPanel}>
        <p className={styles.searchPanelTitle}>Поиск рецепта</p>
        <SearchBar
          searchQuery={searchString}
          onSearch={() => void getAllRecipes(searchString, { pageNumber: 1, pageSize: defaultPageSize })}
          setSearchQuery={setSearchString}
        />
      </div>

      {loading && <p className={styles.emptyRecipeListText}>Загрузка рецептов...</p>}

      <ul className={styles.recipeListBox}>
        {recipes.length > 0 ? (
          recipes.map((recipe) => <RecipeCard recipe={recipe} key={recipe.recipeId} />)
        ) : (
          <p className={styles.emptyRecipeListText}>Рецепты не найдены</p>
        )}
      </ul>

      <div className={styles.loadBtn}>
        <MyButton onClick={handleLoadMore} isPrimary={false} width="309px" height="60px">
          Загрузить еще
        </MyButton>
      </div>
    </div>
  );
};

import MyButton from "../../customComponents/myButton/myButton";
import styles from "./recipesPage.module.scss";
import icon1 from "../../../assets/images/ic-menu.svg";
import icon2 from "../../../assets/images/ic-cook.svg";
import icon3 from "../../../assets/images/ic-chef.svg";
import icon4 from "../../../assets/images/ic-hlop.svg";
import { recipeService } from "../../../services/recipeService";
import { useEffect, useRef, useState } from "react";
import { Recipe } from "../../../types/recipe";
import { Page } from "../../../types/page";
import { useLocation } from "react-router";
import { SearchBar } from "../../customComponents/mySearchBar/searchBar";
import { RecipesPageHeader } from "./recipesPageHeader/recipesPageHeader";
import { RecipeTagCard } from "./tagsPanel/recipeTagCard";
import { useAppStore } from "../../../hooks/useStore";
import { RecipeCard } from "../../customComponents/recipeCard/recipeCard";
import { handleError } from "../../../utils/errorHandler";

type RecipesPageProps = {
  searchQueries: string[];
};

export const RecipesPage = () => {
  const defaultPageSize = 4;
  const location = useLocation();

  const [recipes, setRecipes] = useState<Recipe[]>([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [searchQueries, setSearchQueries] = useState<string[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [isCanLoadMore, setIsCanLoadMore] = useState<boolean>(false);

  const { userId } = useAppStore();

  useEffect(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, [])

  useEffect(() => {
    const initialSearchQuery = (location.state as RecipesPageProps | undefined)?.searchQueries ?? [];
    setSearchQueries(initialSearchQuery);
    setPageNumber(1);
    getAllRecipes(initialSearchQuery, { pageNumber: 1, pageSize: defaultPageSize });
  }, [userId]);

  useEffect(() => {
    setPageNumber(1);
    setIsCanLoadMore(false);
  }, [searchQueries]);

  const getAllRecipes = (searchQuery: string[], currentPage: Page) => {
    setLoading(true);
    recipeService
      .getRecipeList(searchQuery, currentPage)
      .then((response) => {
        if (currentPage.pageNumber === 1) {
          setRecipes(response);
        } else {
          setRecipes((prevRecipes) => [...prevRecipes, ...response]);
        }
        setIsCanLoadMore(response.length === defaultPageSize);
      })
      .catch((error: unknown) => {
        handleError(error);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleSearch = () => {
    setPageNumber(1);
    getAllRecipes(searchQueries, { pageNumber: 1, pageSize: defaultPageSize });
  };

  const handleLoadMore = () => {
    const nextPageNumber = pageNumber + 1;
    setPageNumber(nextPageNumber);
    getAllRecipes(searchQueries, { pageNumber: nextPageNumber, pageSize: defaultPageSize });
  };

  const handleTagClick = (searchString: string) => {
    setSearchQueries([...searchQueries, searchString]);
    setPageNumber(1);
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
          searchQueries={searchQueries}
          onSearch={() => {
            handleSearch();
          }}
          setSearchQueries={setSearchQueries}
        />
      </div>

      <ul className={styles.recipeListBox}>
        {recipes.length > 0
          ? recipes.map((recipe) => <RecipeCard recipe={recipe} key={recipe.recipeId} />)
          : loading && <p className={styles.emptyRecipeListText}>Рецепты не найдены</p>}
      </ul>

      <div className={styles.loadBtn}>
        {isCanLoadMore && (
          <MyButton onClick={handleLoadMore} isPrimary={false} width="309px" height="60px">
            Загрузить еще
          </MyButton>
        )}
      </div>
    </div>
  );
};

import { useCallback, useState } from "react";
import { SearchBar } from "../../customComponents/mySearchBar/searchBar";
import styles from "./searchPanel.module.scss";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../../../constants/constants";

export const SearchPanel = () => {
  const [searchString, setSearchString] = useState("");
  const navigate = useNavigate();

  const handleSearch = useCallback(() => {
    navigate(ROUTES.RECIPES, { state: { searchString } });
  }, [navigate, searchString]);

  return (
    <div className={styles.container}>
      <div className={styles.descriptionBlock}>
        <h2 className={styles.title}>Поиск рецептов</h2>
        <p className={styles.description}>Введите примерное название блюда, а мы по тегам найдем его</p>
      </div>
      <div className={styles.searchPanel}>
        <SearchBar searchQuery={searchString} onSearch={handleSearch} setSearchQuery={setSearchString} />
      </div>
    </div>
  );
};

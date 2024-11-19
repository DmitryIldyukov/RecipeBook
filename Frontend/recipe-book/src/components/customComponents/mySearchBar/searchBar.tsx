import { useEffect, useState } from "react";
import MyButton from "../../customComponents/myButton/myButton";
import styles from "./searchBar.module.scss";
import { Tag } from "../../../types/recipe";
import TagService from "../../../services/tagService";
import muliply from "../../../assets/images/multiply.svg";
import { handleError } from "../../../utils/errorHandler";

type SearchBarProps = {
  onSearch: () => void;
  searchQueries: string[];
  setSearchQueries: (searchQueries: string[]) => void;
};

export const SearchBar = ({ onSearch, searchQueries, setSearchQueries }: SearchBarProps) => {
  const [tags, setTags] = useState<Tag[]>([]);
  const [inputValue, setInputValue] = useState("");
  const tagService = new TagService();

  useEffect(() => {
    tagService
      .getPopularTags(5)
      .then((tags) => {
        setTags(tags);
      })
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при получении популярных тегов");
      });
  }, []);

  const handleRemoveOption = (query: string) => {
    setSearchQueries(searchQueries.filter((s) => s !== query));
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === "Enter" && inputValue.trim() !== "") {
      setSearchQueries([...searchQueries, inputValue.trim()]);
      setInputValue("");
    }
  };

  const handleSearch = () => {
    if (inputValue.trim() !== "" && !searchQueries.includes(inputValue.trim())) {
      setSearchQueries([...searchQueries, inputValue.trim()]);
    }

    onSearch();
  };

  return (
    <div className={styles.container}>
      <div className={styles.searchBar}>
        <div className={styles.line}>
          <div className={styles.inputBox}>
            {searchQueries.length > 0 && (
              <ul className={styles.selectedTags}>
                {searchQueries.map((query, index) => (
                  <li key={index} className={styles.selectedTag}>
                    {query}
                    <button
                      onClick={() => {
                        handleRemoveOption(query);
                      }}
                      className={styles.removeButton}
                    >
                      <img src={muliply} alt="" className={styles.removeIcon} />
                    </button>
                  </li>
                ))}
              </ul>
            )}
            <input
              type="text"
              placeholder="Название Блюда...."
              className={styles.searchBarInput}
              value={inputValue}
              onChange={(e) => {
                setInputValue(e.target.value);
              }}
              onKeyDown={handleKeyDown}
            />
          </div>
          <div className={styles.tagBar}>
            {tags.map((query, index) => (
              <p
                key={index}
                className={styles.tag}
                onClick={() => {
                  setSearchQueries([...searchQueries, query.name]);
                }}
              >
                {query.name}
              </p>
            ))}
          </div>
        </div>
        <MyButton isPrimary={true} onClick={handleSearch} width="152px" height="73px">
          Поиск
        </MyButton>
      </div>
    </div>
  );
};

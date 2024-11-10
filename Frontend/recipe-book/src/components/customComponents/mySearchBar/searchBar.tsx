import { useEffect, useState } from "react";
import MyButton from "../../customComponents/myButton/myButton";
import styles from "./searchBar.module.scss";
import { Tag } from "../../../types/recipe";
import TagService from "../../../services/tagService";

type SearchBarProps = {
  searchQuery: string;
  onSearch: () => void;
  setSearchQuery: (query: string) => void;
};

export const SearchBar = ({ searchQuery, onSearch, setSearchQuery }: SearchBarProps) => {
  const [tags, setTags] = useState<Tag[]>([]);
  const tagService = new TagService();

  useEffect(() => {
    tagService
      .getPopularTags(5)
      .then((tags) => {
        setTags(tags);
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, []);

  return (
    <div className={styles.container}>
      <div className={styles.line}>
        <div className={styles.searchBar}>
          <div className={styles.line}>
            <input
              type="text"
              placeholder="Название Блюда...."
              className={styles.searchBarInput}
              value={searchQuery}
              onChange={(e) => {
                setSearchQuery(e.target.value);
              }}
            />
            <div className={styles.tagBar}>
              {tags.map((tag) => (
                <p
                  key={tag.id}
                  className={styles.tag}
                  onClick={() => {
                    setSearchQuery(tag.name);
                  }}
                >
                  {tag.name}
                </p>
              ))}
            </div>
          </div>
          <MyButton isPrimary={true} onClick={onSearch} width="152px" height="73px">
            Поиск
          </MyButton>
        </div>
      </div>
    </div>
  );
};

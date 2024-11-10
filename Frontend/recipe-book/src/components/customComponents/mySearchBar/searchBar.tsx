import MyButton from "../../customComponents/myButton/myButton";
import styles from "./searchBar.module.scss";

type SearchBarProps = {
  searchQuery: string;
  onSearch: () => void;
  setSearchQuery: (query: string) => void;
};

export const SearchBar = ({ searchQuery, onSearch, setSearchQuery }: SearchBarProps) => {
  const tags = ["Мясо", "Деликатесы", "Пироги", "Рыба"];

  return (
    <div className={styles.container}>
      <div className={styles.line}>
        <div className={styles.searchBar}>
          <input
            type="text"
            placeholder="Название Блюда...."
            className={styles.searchBarInput}
            value={searchQuery}
            onChange={(e) => {
              setSearchQuery(e.target.value);
            }}
          />
          <MyButton isPrimary={true} onClick={onSearch} width="152px" height="73px">
            Поиск
          </MyButton>
        </div>
      </div>
      <div className={styles.line}>
        <div className={styles.tagBar}>
          {tags.map((tag) => (
            <p
              key={tag}
              className={styles.tag}
              onClick={() => {
                setSearchQuery(tag);
              }}
            >
              {tag}
            </p>
          ))}
        </div>
      </div>
    </div>
  );
};

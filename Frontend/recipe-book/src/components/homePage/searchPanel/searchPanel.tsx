import styles from "./searchPanel.module.scss";

export const SearchPanel = () => {
  return (
    <div className={styles.container}>
      <div className={styles.descriptionBlock}>
        <h2 className={styles.title}>Поиск рецептов</h2>
        <p className={styles.description}>Введите примерное название блюда, а мы по тегам найдем его</p>
      </div>
      <div className={styles.searchPanel}>
        <div className={styles.search}>
          <input type="text" placeholder="Название Блюда...." className={styles.searchBar} />
          <button className={styles.searchBtn}>Поиск</button>
        </div>
        <div className={styles.tagBar}>
          <p className={styles.tag}>Мясо</p>
          <p className={styles.tag}>Деликатесы</p>
          <p className={styles.tag}>Пироги</p>
          <p className={styles.tag}>Рыба</p>
        </div>
      </div>
    </div>
  );
};

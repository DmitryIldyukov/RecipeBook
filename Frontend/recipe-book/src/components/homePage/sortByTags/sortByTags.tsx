import styles from "./sortByTags.module.scss";
import icon1 from "../../../assets/ic-menu.svg";
import icon2 from "../../../assets/ic-cook.svg";
import icon3 from "../../../assets/ic-chef.svg";
import icon4 from "../../../assets/ic-hlop.svg";

export const SortByTags = () => {
  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Умная сортировка по тегам</h2>
      <p className={styles.description}>
        Добавляй рецепты и указывай наиболее популярные теги. Это позволит быстро находить любые категории.
      </p>
      <div className={styles.cardsPanel}>
        <div className={styles.content}>
          <div className={styles.iconBox}>
            <img src={icon1} alt="Простые блюда" className={styles.icon} />
          </div>
          <h3 className={styles.cardTitle}>Простые блюда</h3>
          <p className={styles.cardDescription}>Время приготвления таких блюд не более 1 часа</p>
        </div>
        <div className={styles.content}>
          <div className={styles.iconBox}>
            <img src={icon2} alt="Детское" className={styles.icon} />
          </div>
          <h3 className={styles.cardTitle}>Детское</h3>
          <p className={styles.cardDescription}>Самые полезные блюда которые можно детям любого возраста</p>
        </div>
        <div className={styles.content}>
          <div className={styles.iconBox}>
            <img src={icon3} alt="От шеф-поваров" className={styles.icon} />
          </div>
          <h3 className={styles.cardTitle}>От шеф-поваров</h3>
          <p className={styles.cardDescription}>Требуют умения, времени и терпения, зато как в ресторане</p>
        </div>
        <div className={styles.content}>
          <div className={styles.iconBox}>
            <img src={icon4} alt="На праздник" className={styles.icon} />
          </div>
          <h3 className={styles.cardTitle}>На Праздник</h3>
          <p className={styles.cardDescription}>Чем удивить гостей, чтобы все были сыты за праздничным столом</p>
        </div>
      </div>
    </div>
  );
};

import styles from "./sortByTags.module.scss";
import icon1 from "../../../../assets/images/ic-menu.svg";
import icon2 from "../../../../assets/images/ic-cook.svg";
import icon3 from "../../../../assets/images/ic-chef.svg";
import icon4 from "../../../../assets/images/ic-hlop.svg";
import { useNavigate } from "react-router";
import { ROUTES } from "../../../../constants/constants";

const tags = [
  {
    title: "Простые блюда",
    description: "Время приготовления таких блюд не более 1 часа",
    icon: icon1,
    searchString: "Простые блюда",
  },
  {
    title: "Детское",
    description: "Самые полезные блюда, которые можно детям любого возраста",
    icon: icon2,
    searchString: "Детское",
  },
  {
    title: "От шеф-поваров",
    description: "Требуют умения, времени и терпения, зато как в ресторане",
    icon: icon3,
    searchString: "От шеф-поваров",
  },
  {
    title: "На Праздник",
    description: "Чем удивить гостей, чтобы все были сыты за праздничным столом",
    icon: icon4,
    searchString: "На праздник",
  },
];

export const SortByTags = () => {
  const navigate = useNavigate();

  const handleTagClick = (searchString: string) => {
    const searchQueries = [searchString];
    navigate(ROUTES.RECIPES, { state: { searchQueries } });
  };

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Умная сортировка по тегам</h2>
      <p className={styles.description}>
        Добавляй рецепты и указывай наиболее популярные теги. Это позволит быстро находить любые категории.
      </p>
      <div className={styles.cardsPanel}>
        {tags.map((tag, index) => (
          <div
            key={index}
            className={styles.content}
            onClick={() => {
              handleTagClick(tag.searchString);
            }}
          >
            <div className={styles.iconBox}>
              <img src={tag.icon} alt={tag.title} className={styles.icon} />
            </div>
            <h3 className={styles.cardTitle}>{tag.title}</h3>
            <p className={styles.cardDescription}>{tag.description}</p>
          </div>
        ))}
      </div>
    </div>
  );
};

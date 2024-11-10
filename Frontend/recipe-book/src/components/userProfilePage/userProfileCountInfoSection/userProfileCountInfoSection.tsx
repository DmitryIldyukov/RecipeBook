import styles from "./userProfileCountInfoSection.module.scss";
import icon from "../../../assets/ic-menu.svg";

type UserProfileCountInfoProps = {
  recipesCount?: number;
  likesCount?: number;
  favoritesCount?: number;
};

export const UserProfileCountInfoSection = ({
  recipesCount,
  likesCount,
  favoritesCount,
}: UserProfileCountInfoProps) => {
  const cards = [
    {
      title: "Всего рецептов",
      count: recipesCount ?? 0,
    },
    {
      title: "Всего лайков",
      count: likesCount ?? 0,
    },
    {
      title: "В избранных",
      count: favoritesCount ?? 0,
    },
  ];

  return (
    <div className={styles.container}>
      {cards.map((card, index) => (
        <div key={index} className={styles.card}>
          <div className={styles.content}>
            <div className={styles.iconBox}>
              <img src={icon} alt="иконка" className={styles.icon} />
            </div>
            <p className={styles.title}>{card.title}</p>
          </div>
          <p className={styles.count}>{card.count}</p>
        </div>
      ))}
    </div>
  );
};

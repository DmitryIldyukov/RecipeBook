import styles from "./recipeTagCard.module.scss";

type RecipeTagCardProps = {
  title: string;
  icon: string;
  onClick: () => void;
};

export const RecipeTagCard = ({ title, icon, onClick }: RecipeTagCardProps) => {
  return (
    <div className={styles.card} onClick={onClick}>
      <div className={styles.cardImage}>
        <img src={icon} alt={title} />
      </div>
      <p className={styles.cardTitle}>{title}</p>
    </div>
  );
};

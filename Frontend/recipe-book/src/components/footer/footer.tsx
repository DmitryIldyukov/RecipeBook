import styles from "./footer.module.scss";

export const Footer = () => {
  return (
    <div className={styles.content}>
      <div className={styles.container}>
        <p className={styles.title}>Recipes</p>
        <p className={styles.description}>© Recipes 2021</p>
      </div>
    </div>
  );
};

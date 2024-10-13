import styles from "./homePage.module.scss";
import introImage from "../../assets/header image.jpg";
import addImage from "../../assets/add.svg";

export const HomePage = () => {
  return (
    <div className={styles.container}>
      <div className={styles.intro}>
        <div className={styles.introInfo}>
          <h1 className={styles.title}>Готовь и делись рецептами</h1>
          <p className={styles.description}>
            Никаких кулинарных книг и блокнотов! Храни все
            <br />
            любимые рецепты в одном месте.
          </p>
          <div className={styles.actionButtons}>
            <button className={styles.primaryBtn}>
              <img src={addImage} alt="" /> Добавить рецепт
            </button>
            <button className={styles.secondaryBtn}>Войти</button>
          </div>
        </div>
        <img src={introImage} alt="header image" className={styles.introImage} width={602} height={800} />
      </div>
    </div>
  );
};

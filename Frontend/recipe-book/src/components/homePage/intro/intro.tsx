import styles from "./intro.module.scss";
import introImage from "../../../assets/header image.jpg";
import addImage from "../../../assets/add.svg";
import { MyButton } from "../../customComponents/myButton/myButton";

export const Intro = () => {
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
            <MyButton
              onClick={() => {
                console.log("add recipe");
              }}
              isPrimary={true}
              width="278px"
              height="60px"
            >
              <img src={addImage} alt="add recipe" /> Добавить рецепт
            </MyButton>
            <MyButton
              onClick={() => {
                console.log("Login");
              }}
              isPrimary={false}
              width="216px"
              height="60px"
            >
              Войти
            </MyButton>
          </div>
        </div>
        <img src={introImage} alt="header image" className={styles.introImage} width={602} height={800} />
      </div>
    </div>
  );
};

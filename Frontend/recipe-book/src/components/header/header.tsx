import styles from "./header.module.scss";
import loginImg from "../../assets/login.svg";
import logoutImg from "../../assets/exit_to_app.svg";
import { useAppStore } from "../../store/store";
import { useLocation, useNavigate } from "react-router-dom";
import { homePage, recipePage, favoritesPage } from "../../constants/pathConstants";

export const Header = () => {
  const location = useLocation();
  const navigate = useNavigate();

  const { isLogged, logout, login } = useAppStore();

  const logOut = () => {
    if (isLogged) {
      console.log("Logging out...");
      logout();
    } else {
      console.log("Logging in...");
      login();
    }
  };

  return (
    <div className={styles.content}>
      <div className={styles.container}>
        <div className={styles.menu}>
          <button
            className={styles.mainBtn}
            onClick={() => {
              navigate(homePage);
            }}
          >
            Recipes
          </button>
          <div className={styles.menuButtons}>
            <button
              className={`${styles.menuBtn} ${location.pathname === homePage ? styles.bold : styles.regular}`}
              onClick={() => {
                navigate(homePage);
              }}
            >
              Главная
            </button>
            <button
              className={`${styles.menuBtn} ${location.pathname === recipePage ? styles.bold : styles.regular}`}
              onClick={() => {
                navigate(recipePage);
              }}
            >
              Рецепты
            </button>
            <button
              className={`${styles.menuBtn} ${location.pathname === favoritesPage ? styles.bold : styles.regular}`}
              onClick={() => {
                navigate(favoritesPage);
              }}
            >
              Избранное
            </button>
          </div>
        </div>
        <div className={styles.loginContainer}>
          <img src={loginImg} alt="login" />
          {!isLogged ? (
            <button className={styles.loginBtn} onClick={logOut}>
              Войти
            </button>
          ) : (
            <>
              <button className={styles.loginBtn}>Привет, Татьяна</button>
              <span className={styles.line}>|</span>
              <img className={styles.logoutBtn} src={logoutImg} alt="logout" onClick={logOut} />
            </>
          )}
        </div>
      </div>
    </div>
  );
};

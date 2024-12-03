import styles from "./header.module.scss";
import loginImg from "../../assets/images/login.svg";
import logoutImg from "../../assets/images/exit_to_app.svg";
import { useAppStore } from "../../hooks/useStore";
import { useLocation, useNavigate } from "react-router-dom";
import { ROUTES } from "../../constants/constants";
import { useEffect, useState } from "react";
import { User } from "../../types/user";
import { usePopupStore } from "../../hooks/usePopupStore";
import { userService } from "../../services/userService";
import { handleError } from "../../utils/errorHandler";

export const Header = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const [user, setUser] = useState<User | null>(null);

  const { userId, isAuth, logout } = useAppStore();
  const { setIsLoginPopupOpen } = usePopupStore();

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (isAuth && userId) {
          await userService.getUser(userId).then((user) => {
            setUser(user);
          });
        }
      } catch (error) {
        handleError(error, "Произошла ошибка при загрузке данных пользователя");
      }
    };

    void fetchData();
  }, [isAuth]);

  const navigateToLogin = () => {
    setIsLoginPopupOpen(true);
  };

  return (
    <div className={styles.content}>
      <div className={styles.container}>
        <div className={styles.menu}>
          <button
            className={styles.mainBtn}
            onClick={() => {
              navigate(ROUTES.HOME);
            }}
          >
            Recipes
          </button>
          <div className={styles.menuButtons}>
            <button
              className={`${styles.menuBtn} ${location.pathname === ROUTES.HOME ? styles.bold : styles.regular}`}
              onClick={() => {
                navigate(ROUTES.HOME);
              }}
            >
              Главная
            </button>
            <button
              className={`${styles.menuBtn} ${
                location.pathname === ROUTES.RECIPES ||
                location.pathname.includes(ROUTES.RECIPE_INFO) ||
                location.pathname.includes(ROUTES.EDIT_RECIPE) ||
                location.pathname.includes(ROUTES.PROFILE)
                  ? styles.bold
                  : styles.regular
              }`}
              onClick={() => {
                navigate(ROUTES.RECIPES);
              }}
            >
              Рецепты
            </button>
            <button
              className={`${styles.menuBtn} ${location.pathname === ROUTES.FAVORITES ? styles.bold : styles.regular}`}
              onClick={() => {
                navigate(ROUTES.FAVORITES);
              }}
            >
              Избранное
            </button>
          </div>
        </div>
        <div className={styles.loginContainer}>
          <img src={loginImg} alt="login" />
          {!userId ? (
            <button className={styles.loginBtn} onClick={navigateToLogin}>
              Войти
            </button>
          ) : (
            <>
              <button
                className={styles.loginBtn}
                onClick={() => {
                  navigate(`${ROUTES.PROFILE}/${userId.toString()}`);
                }}
              >
                Привет, {user?.name}
              </button>
              <span className={styles.line}>|</span>
              <img className={styles.logoutBtn} src={logoutImg} alt="logout" onClick={logout} />
            </>
          )}
        </div>
      </div>
    </div>
  );
};

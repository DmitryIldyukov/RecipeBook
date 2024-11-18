import MyButton from "../../customComponents/myButton/myButton";
import closeIcon from "../../../assets/images/close.svg";
import styles from "./loginPopup.module.scss";
import { usePopupStore } from "../../../hooks/usePopupStore";
import { authService } from "../../../services/authService";
import { LoginInfo } from "../../../types/auth";
import { useState } from "react";
import { useAppStore } from "../../../hooks/useStore";
import { handleError } from "../../../utils/errorHandler";

export const LoginPopup = () => {
  const { login } = useAppStore();
  const { setIsLoginPopupOpen, setIsRegistrationPopupOpen } = usePopupStore();

  const [loginData, setLogin] = useState("");
  const [password, setPassword] = useState("");

  const handleClosePopup = () => {
    setIsLoginPopupOpen(false);
  };

  const navigateToRegistration = () => {
    setIsLoginPopupOpen(false);
    setIsRegistrationPopupOpen(true);
  };

  const handleLogin = () => {
    const data: LoginInfo = { login: loginData, password: password };

    authService
      .login(data)
      .then((response) => {
        login(response);
        handleClosePopup();
      })
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при авторизации");
      });
  };

  return (
    <div className={styles.container}>
      <div className={styles.content}>
        <button onClick={handleClosePopup} className={styles.closeBtn}>
          <img src={closeIcon} alt="close" className={styles.closeIcon} />
        </button>
        <p className={styles.title}>Войти</p>
        <form className={styles.form}>
          <input
            type="text"
            placeholder="Логин"
            className={styles.input}
            id="login"
            value={loginData}
            onChange={(e) => {
              setLogin(e.target.value);
            }}
          />
          <input
            type="password"
            placeholder="Пароль"
            className={styles.input}
            id="password"
            value={password}
            onChange={(e) => {
              setPassword(e.target.value);
            }}
          />

          <div className={styles.buttons}>
            <MyButton isPrimary={true} onClick={handleLogin} width="278px" height="60px">
              Войти
            </MyButton>
            <MyButton isPrimary={false} onClick={handleClosePopup} width="278px" height="60px">
              Отмена
            </MyButton>
          </div>
        </form>
        <div className={styles.registration}>
          <button onClick={navigateToRegistration} className={styles.registrationLink}>
            У меня еще нет аккаунта
          </button>
        </div>
      </div>
    </div>
  );
};

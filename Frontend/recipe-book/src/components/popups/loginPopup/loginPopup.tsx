import MyButton from "../../customComponents/myButton/myButton";
import closeIcon from "../../../assets/close.svg";
import styles from "./loginPopup.module.scss";
import { usePopupStore } from "../../../hooks/usePopupStore";
import AuthService from "../../../services/authService";
import { LoginInfo } from "../../../types/auth";
import { useState } from "react";
import { useAppStore } from "../../../hooks/useStore";

export const LoginPopup = () => {
  const authService = new AuthService();

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

  const handleLogin = async () => {
    const data: LoginInfo = { login: loginData, password: password };

    try {
      const response = await authService.Login(data);
      login(response);
      handleClosePopup();
    } catch (error) {
      console.error(error);
    }
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
            <MyButton isPrimary={true} onClick={() => void handleLogin()} width="278px" height="60px">
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

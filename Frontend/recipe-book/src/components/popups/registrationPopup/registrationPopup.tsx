import MyButton from "../../customComponents/myButton/myButton";
import closeIcon from "../../../assets/close.svg";
import styles from "./registrationPopup.module.scss";
import { usePopupStore } from "../../../hooks/usePopupStore";
import { useState } from "react";
import { LoginInfo, RegistrationInfo } from "../../../types/auth";
import { useAppStore } from "../../../hooks/useStore";
import { authService } from "../../../services/authService";
import { handleError } from "../../../utils/errorHandler";
import toast from "react-hot-toast";

export const RegistrationPopup = () => {
  const { setIsRegistrationPopupOpen, setIsLoginPopupOpen } = usePopupStore();
  const { login } = useAppStore();

  const [name, setName] = useState("");
  const [loginData, setLoginData] = useState("");
  const [password, setPassword] = useState("");
  const [chechPassword, setChechPassword] = useState("");

  const handleClosePopup = () => {
    setIsRegistrationPopupOpen(false);
  };

  const navigateToLogin = () => {
    setIsRegistrationPopupOpen(false);
    setIsLoginPopupOpen(true);
  };

  const handleRegistration = async () => {
    const data: RegistrationInfo = { name: name, login: loginData, password: password };

    if (password !== chechPassword) {
      toast.error("Пароли не совпадают");
      return;
    }

    await authService
      .Registration(data)
      .then(handleLogin)
      .catch((error: unknown) => {
        handleError(error, "Произошла ошибка при регистрации");
      });
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
        <p className={styles.title}>Регистрация</p>
        <form className={styles.form}>
          <input
            type="text"
            placeholder="Имя"
            className={styles.input}
            id="name"
            value={name}
            onChange={(e) => {
              setName(e.target.value);
            }}
          />
          <input
            type="text"
            placeholder="Логин"
            className={styles.input}
            id="login"
            value={loginData}
            onChange={(e) => {
              setLoginData(e.target.value);
            }}
          />

          <div className={styles.passwordContainer}>
            <div className={styles.password}>
              <input
                type="password"
                placeholder="Пароль"
                className={styles.passInput}
                id="password"
                value={password}
                onChange={(e) => {
                  setPassword(e.target.value);
                }}
              />
              <span className={styles.description}>Минимум 8 символов</span>
            </div>
            <input
              type="password"
              placeholder="Пароль"
              className={styles.input}
              id="password"
              value={chechPassword}
              onChange={(e) => {
                setChechPassword(e.target.value);
              }}
            />
          </div>

          <div className={styles.buttons}>
            <MyButton isPrimary={true} onClick={() => void handleRegistration()} width="278px" height="60px">
              Зарегистрироваться
            </MyButton>
            <MyButton isPrimary={false} onClick={handleClosePopup} width="278px" height="60px">
              Отмена
            </MyButton>
          </div>
        </form>
        <div className={styles.registration}>
          <button onClick={navigateToLogin} className={styles.registrationLink}>
            У меня уже есть аккаунт
          </button>
        </div>
      </div>
    </div>
  );
};

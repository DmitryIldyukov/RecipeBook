import MyButton from "../../customComponents/myButton/myButton";
import closeIcon from "../../../assets/images/close.svg";
import styles from "./loginPopup.module.scss";
import { usePopupStore } from "../../../hooks/usePopupStore";
import { LoginInfo } from "../../../types/auth";
import { useState } from "react";
import { useAppStore } from "../../../hooks/useStore";
import { handleError } from "../../../utils/errorHandler";

export const LoginPopup = () => {
  const { login } = useAppStore();
  const { setIsLoginPopupOpen, setIsRegistrationPopupOpen } = usePopupStore();

  const [loginData, setLogin] = useState("");
  const [password, setPassword] = useState("");
  const [errors, setErrors] = useState<{ login?: string; password?: string }>({});

  const handleClosePopup = () => {
    setIsLoginPopupOpen(false);
  };

  const navigateToRegistration = () => {
    setIsLoginPopupOpen(false);
    setIsRegistrationPopupOpen(true);
  };

  const validate = () => {
    const newErrors: { login?: string; password?: string } = {};

    if (!loginData) {
      newErrors.login = "Логин обязателен";
    }

    if (password.length < 8) {
      newErrors.password = "Пароль должен содержать минимум 8 символов";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleLogin = () => {
    if (!validate()) {
      return;
    }

    const data: LoginInfo = { login: loginData, password: password };

    login(data)
      .then(() => {
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
          <div className={styles.formInputs}>
            <div className={styles.inputBlock}>
              <input
                type="text"
                placeholder="Логин"
                className={styles.input}
                id="login"
                value={loginData}
                onChange={(e) => {
                  setLogin(e.target.value);
                  setErrors((prev) => ({ ...prev, login: "" }));
                }}
              />
              {errors.login && <div className={styles.errorText}>{errors.login}</div>}
            </div>

            <div className={styles.inputBlock}>
              <input
                type="password"
                placeholder="Пароль"
                className={styles.input}
                id="password"
                value={password}
                onChange={(e) => {
                  setPassword(e.target.value);
                  setErrors((prev) => ({ ...prev, password: "" }));
                }}
              />
              {errors.password && <div className={styles.errorText}>{errors.password}</div>}
            </div>
          </div>

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

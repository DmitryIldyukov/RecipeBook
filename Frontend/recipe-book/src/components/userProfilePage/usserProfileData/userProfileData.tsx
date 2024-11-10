import { useEffect, useState } from "react";
import styles from "./userProfileData.module.scss";
import pencil from "../../../assets/pencil.svg";
import { User } from "../../../types/user";
import MyButton from "../../customComponents/myButton/myButton";
import UserService from "../../../services/userService";
import { useAppStore } from "../../../hooks/useStore";
import eyeHidden from "../../../assets/eyeHidden.svg";
import eyeVisible from "../../../assets/eyeVisible.svg";
import { handleError } from "../../../utils/errorHandler";
import toast from "react-hot-toast";

type UserProfileDataProps = {
  user: User | null;
};

export const UserProfileData = ({ user }: UserProfileDataProps) => {
  const userService = new UserService();

  const [name, setName] = useState(user?.name ?? "");
  const [login, setLogin] = useState(user?.login ?? "");
  const [password, setPassword] = useState("");
  const [information, setInformation] = useState(user?.information ?? "");
  const [isEditingMode, setIsEditingMode] = useState(false);
  const [passwordIsHidden, setPasswordIsHidden] = useState(true);

  const { userId } = useAppStore();

  useEffect(() => {
    if (user) {
      setName(user.name);
      setLogin(user.login);
      setInformation(user.information);
    }
  }, [user]);

  const handleEditingMode = () => {
    setPassword("");
    setIsEditingMode(!isEditingMode);
  };

  const saveUser = () => {
    if (userId) {
      userService
        .saveUser(userId, {
          name: name,
          login: login,
          password: password,
          information: information,
        })
        .then(() => {
          toast.success("Данные изменены");
          handleEditingMode();
        })
        .catch((error: unknown) => {
          handleError(error, "Произошла ошибка при сохранении данных профиля");
        });
    }
  };

  const handleSaveUser = () => {
    saveUser();
  };

  return (
    <div className={styles.container}>
      {!isEditingMode && (
        <button className={styles.editBtn} onClick={handleEditingMode}>
          <img src={pencil} alt="edit" className={styles.editIcon} />
        </button>
      )}
      <div className={styles.data}>
        <div className={styles.inputContainer}>
          <div className={styles.relative}>
            <input
              id={"name"}
              type="text"
              className={styles.inputText}
              value={name}
              disabled={!isEditingMode}
              minLength={2}
              maxLength={30}
              onChange={(e) => {
                setName(e.target.value);
              }}
            />
            <label htmlFor={"name"} className={styles.label}>
              Имя
            </label>
          </div>
          <div className={styles.relative}>
            <input
              id={"login"}
              type="text"
              className={styles.inputText}
              value={login}
              disabled={!isEditingMode}
              minLength={2}
              maxLength={50}
              onChange={(e) => {
                setLogin(e.target.value);
              }}
            />
            <label htmlFor={"login"} className={styles.label}>
              Логин
            </label>
          </div>
          <div>
            <div className={styles.relative}>
              <input
                id={"password"}
                type={passwordIsHidden ? "password" : "text"}
                className={styles.inputTextPassword}
                value={isEditingMode ? password : "********"}
                disabled={!isEditingMode}
                minLength={8}
                onChange={(e) => {
                  setPassword(e.target.value);
                }}
              />
              <button
                onClick={() => {
                  setPasswordIsHidden(!passwordIsHidden);
                }}
                className={styles.eyeIconBox}
                disabled={!isEditingMode}
              >
                <img src={passwordIsHidden ? eyeHidden : eyeVisible} className={styles.eyeIcon} />
              </button>
            </div>
          </div>
        </div>
        <textarea
          placeholder="Напишите немного о себе"
          className={styles.area}
          value={information}
          disabled={!isEditingMode}
          onChange={(e) => {
            setInformation(e.target.value);
          }}
        />
      </div>
      {isEditingMode && (
        <div className={styles.buttons}>
          <MyButton isPrimary={false} onClick={handleEditingMode} width="140px" height="60px">
            Отмена
          </MyButton>
          <MyButton isPrimary={true} onClick={handleSaveUser} width="140px" height="60px">
            Сохранить
          </MyButton>
        </div>
      )}
    </div>
  );
};

import styles from "./myButton.module.scss";
import React from "react";

type MyButtonProps = {
  onClick: () => void;
  children: React.ReactNode;
  isPrimary: boolean;
  width: string;
  height: string;
};

export const MyButton = ({ onClick, children, isPrimary, width, height }: MyButtonProps) => {
  return (
    <button
      onClick={onClick}
      className={isPrimary ? styles.myBtnPrimary : styles.myBtnSecondary}
      style={{ width, height }}
    >
      {children}
    </button>
  );
};

export default MyButton;

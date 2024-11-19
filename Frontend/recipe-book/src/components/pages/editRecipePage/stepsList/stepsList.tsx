import React from "react";
import { Step } from "../../../../types/recipe";
import styles from "./stepsList.module.scss";
import MyButton from "../../../customComponents/myButton/myButton";
import addImage from "../../../../assets/images/add-ptimary.svg";
import closeIcon from "../../../../assets/images/close.svg";

type StepsListProps = {
  steps: Step[];
  setSteps: React.Dispatch<React.SetStateAction<Step[]>>;
};

export const StepsList = ({ steps, setSteps }: StepsListProps) => {
  const handleStepChange = (index: number, value: string) => {
    const updatedSteps = [...steps];
    updatedSteps[index].description = value;
    setSteps(updatedSteps);
  };

  const addStep = () => {
    setSteps([...steps, { id: undefined, description: "" }]);
  };

  const removeStep = (index: number) => {
    if (steps.length > 1) {
      setSteps(steps.filter((_, i) => i !== index));
    }
  };

  return (
    <ul className={styles.stepsBlock}>
      {steps.length > 0 &&
        steps.map((step, index) => (
          <li key={step.id ?? index.toString()} className={styles.stepInfo}>
            <div className={styles.stepTitleBox}>
              <p className={styles.stepTitle}>Шаг {index + 1}</p>
              <button
                type="button"
                className={styles.closeIconBox}
                onClick={() => {
                  removeStep(index);
                }}
              >
                <img src={closeIcon} alt="close" className={styles.closeIcon} />
              </button>
            </div>
            <textarea
              name=""
              id=""
              placeholder="Описание шага"
              className={styles.stepDescription}
              value={step.description}
              onChange={(e) => {
                handleStepChange(index, e.target.value);
              }}
            />
          </li>
        ))}
      <MyButton isPrimary={false} onClick={addStep} width="380px" height="60px">
        <img src={addImage} alt="add recipe" /> Добавить шаг
      </MyButton>
    </ul>
  );
};

import React, { useRef } from "react";
import styles from "./imageUploader.module.scss";
import downloadIcon from "../../../../assets/images/cloud-download.svg";

type ImageUploaderProps = {
  imageFile: File | null;
  setImageFile: React.Dispatch<React.SetStateAction<File | null>>;
  setImageName: React.Dispatch<React.SetStateAction<string>>;
};

export const ImageUploader = ({ imageFile, setImageFile, setImageName }: ImageUploaderProps) => {
  const fileInputRef = useRef<HTMLInputElement | null>(null);

  const handleUploadClick = () => fileInputRef.current?.click();

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setImageFile(file);
      setImageName(file.name);
    }
  };

  return (
    <div className={styles.uploadInput} onClick={handleUploadClick}>
      {imageFile && <img src={URL.createObjectURL(imageFile)} alt="recipe" className={styles.image} />}
      <input
        type="file"
        accept=".png, .jpg, .jpeg"
        id="recipeImage"
        className={styles.hidden}
        ref={fileInputRef}
        onChange={handleFileChange}
      />
      {!imageFile && (
        <div className={styles.uploadPlaceholder}>
          <img src={downloadIcon} alt="download" className={styles.downloadIcon} />
          <span className={styles.downloadText}>Загрузите фото готового блюда</span>
        </div>
      )}
    </div>
  );
};

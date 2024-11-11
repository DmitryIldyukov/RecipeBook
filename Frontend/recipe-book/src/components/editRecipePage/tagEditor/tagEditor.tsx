import { ChangeEvent, useEffect, useRef, useState } from "react";
import { Tag } from "../../../types/recipe";
import TagService from "../../../services/tagService";
import styles from "./tagEditor.module.scss";
import muliply from "../../../assets/multiply.svg";

type TagEditorProps = {
  tags: Tag[];
  setTags: React.Dispatch<React.SetStateAction<Tag[]>>;
};

export const TagEditor = ({ tags, setTags }: TagEditorProps) => {
  const tagService = new TagService();

  const [allTags, setAllTags] = useState<Tag[]>([]);
  const [inputValue, setInputValue] = useState<string>("");
  const [filteredTags, setFilteredTags] = useState<Tag[]>([]);
  const [showDropdown, setShowDropdown] = useState<boolean>(false);

  const tagEditorRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    tagService
      .getTags()
      .then((allTags) => {
        setAllTags(allTags);
        setFilteredTags(allTags.filter((tag) => !tags.some((t) => t.id === tag.id)));
      })
      .catch((error: unknown) => {
        console.error(error);
      });
  }, [tags]);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (tagEditorRef.current && !tagEditorRef.current.contains(event.target as Node)) {
        setShowDropdown(false);
      }
    };

    document.addEventListener("click", handleClickOutside, true);

    return () => {
      document.removeEventListener("click", handleClickOutside, true);
    };
  }, []);

  const handleInputChange = (event: ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setInputValue(value);
    setFilteredTags(
      allTags.filter(
        (tag) => tag.name.toLowerCase().includes(value.toLowerCase()) && !tags.some((t) => t.id === tag.id),
      ),
    );
    setShowDropdown(true);
  };

  const handleOptionClick = (tag: Tag) => {
    if (!tags.some((t) => t.id === tag.id)) {
      setTags((prevTags) => [...prevTags, tag]);
    }
    setFilteredTags(allTags.filter((tag) => !tags.some((t) => t.id === tag.id)));
    setInputValue("");
    setShowDropdown(false);
  };

  const handleRemoveOption = (tag: Tag) => {
    setTags((prevTags) => prevTags.filter((t) => t.id !== tag.id));
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === "Enter" && inputValue.trim() !== "") {
      const newTag: Tag = { id: undefined, name: inputValue.trim() };
      if (!tags.some((t) => t.name === newTag.name)) {
        setTags((prevTags) => [...prevTags, newTag]);
      }
      setInputValue("");
      setShowDropdown(false);
    }
  };

  return (
    <div className={styles.tagEditor} ref={tagEditorRef}>
      <div className={styles.inputBox}>
        {tags.length > 0 && (
          <ul className={styles.selectedTags}>
            {tags.map((tag, index) => (
              <li key={index} className={styles.selectedTag}>
                {tag.name}
                <button
                  type="button"
                  onClick={() => {
                    handleRemoveOption(tag);
                  }}
                >
                  <img src={muliply} alt="" className={styles.removeIcon} />
                </button>
              </li>
            ))}
          </ul>
        )}
        <input
          type="text"
          value={inputValue}
          className={styles.tagInput}
          onChange={handleInputChange}
          onKeyDown={handleKeyDown}
          onFocus={() => {
            setShowDropdown(true);
          }}
          placeholder="Добавить теги"
        />
      </div>
      {showDropdown && filteredTags.length > 0 && (
        <ul className={styles.dropdown}>
          {filteredTags.map((tag) => (
            <li
              className={styles.dropdownOption}
              key={tag.id}
              onClick={() => {
                setFilteredTags(allTags.filter((tag) => !tags.some((t) => t.id === tag.id)));
                handleOptionClick(tag);
              }}
            >
              {tag.name}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

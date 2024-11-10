import { Intro } from "./intro/intro";
import { RecipeOfDay } from "./recipeOfDay/recipeOfDay";
import { SortByTags } from "./sortByTags/sortByTags";
import { SearchPanel } from "./searchPanel/searchPanel";

export const HomePage = () => {
  return (
    <>
      <Intro />
      <SortByTags />
      <RecipeOfDay />
      <SearchPanel />
    </>
  );
};

import { Intro } from "./intro/intro";
import { RecipeOfDay } from "./recipeOfDay/recipeOfDay";
import { SearchPanel } from "./searchPanel/searchPanel";
import { SortByTags } from "./sortByTags/sortByTags";

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

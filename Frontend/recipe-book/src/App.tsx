import "./App.scss";
import { Footer } from "./components/footer/footer";
import { Header } from "./components/header/header";
import { HomePage } from "./components/homePage/homePage";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { RecipesPage } from "./components/recipesPage/recipesPage";
import { ROUTES } from "./constants/constants";
import { RecipeFullInfo } from "./components/recipesPage/recipeFullInfo/recipeFullInfo";
import { FavoriteList } from "./components/favoritesPage/favoriteList";
import { usePopupStore } from "./hooks/usePopupStore";
import { RegistrationPopup } from "./components/popups/registrationPopup/registrationPopup";
import { EditRecipePage } from "./components/editRecipePage/editRecipePage";
import { UserProfilePage } from "./components/userProfilePage/userProfilePage";
import { LoginOrRegistrationPopup } from "./components/popups/loginOrRegistraionPopup/loginOrRegistrationPopup";
import { LoginPopup } from "./components/popups/loginPopup/loginPopup";

function App() {

  return (
    <BrowserRouter>
      <div className="app">
        <Header />
      </div>
    </BrowserRouter>
  );
}

export default App;

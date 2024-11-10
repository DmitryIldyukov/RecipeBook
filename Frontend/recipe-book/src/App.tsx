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
  const { isLoginPopupOpen, isRegistrationPopupOpen, isLoginOrRegistraionPopupOpen } = usePopupStore();

  return (
    <BrowserRouter>
      <div className="app">
        <Header />
        <div className="container">
          <Routes>
            <Route path={ROUTES.HOME} element={<HomePage />} />
            <Route path={ROUTES.RECIPES} element={<RecipesPage />} />
            <Route path={`${ROUTES.RECIPE_INFO}/:recipeId`} element={<RecipeFullInfo />} />
            <Route path="*" element={<Navigate to={ROUTES.HOME} replace />} />
          </Routes>
        </div>
        <Footer />
        {isLoginOrRegistraionPopupOpen && <LoginOrRegistrationPopup />}
        {isLoginPopupOpen && <LoginPopup />}
        {isRegistrationPopupOpen && <RegistrationPopup />}
      </div>
    </BrowserRouter>
  );
}

export default App;

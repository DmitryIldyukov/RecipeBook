import "./App.scss";
import { Footer } from "./components/footer/footer";
import { Header } from "./components/header/header";
import { HomePage } from "./components/pages/homePage/homePage";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { RecipesPage } from "./components/pages/recipesPage/recipesPage";
import { ROUTES } from "./constants/constants";
import { RecipeFullInfo } from "./components/pages/recipesPage/recipeFullInfo/recipeFullInfo";
import { FavoriteList } from "./components/pages/favoritesPage/favoriteList";
import { usePopupStore } from "./hooks/usePopupStore";
import { RegistrationPopup } from "./components/popups/registrationPopup/registrationPopup";
import { EditRecipePage } from "./components/pages/editRecipePage/editRecipePage";
import { UserProfilePage } from "./components/pages/userProfilePage/userProfilePage";
import { LoginOrRegistrationPopup } from "./components/popups/loginOrRegistraionPopup/loginOrRegistrationPopup";
import { LoginPopup } from "./components/popups/loginPopup/loginPopup";
import { Toaster } from "react-hot-toast";

function App() {
  const { isLoginPopupOpen, isRegistrationPopupOpen, isLoginOrRegistraionPopupOpen } = usePopupStore();

  return (
    <BrowserRouter>
      <Toaster
        toastOptions={{
          style: {
            fontFamily: "Montserrat",
            fontSize: "14px",
          },
        }}
        position="top-center"
        reverseOrder={false}
      />
      <div className="app">
        <Header />
        <div className="container">
          <Routes>
            <Route path={ROUTES.HOME} element={<HomePage />} />
            <Route path={ROUTES.RECIPES} element={<RecipesPage />} />
            <Route path={`${ROUTES.RECIPE_INFO}/:recipeId`} element={<RecipeFullInfo />} />
            <Route path={`${ROUTES.EDIT_RECIPE}/:recipeId?`} element={<EditRecipePage />} />
            <Route path={`${ROUTES.PROFILE}/:userId`} element={<UserProfilePage />} />
            <Route path={ROUTES.FAVORITES} element={<FavoriteList />} />
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

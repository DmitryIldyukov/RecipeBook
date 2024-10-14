import "./App.scss";
import { Footer } from "./components/footer/footer";
import { Header } from "./components/header/header";
import { HomePage } from "./components/homePage/homePage";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { homePage, recipeListPage } from "./constants/pathConstants";
import { RecipesPage } from "./components/recipesPage/recipeListPage";

function App() {
  return (
    <BrowserRouter>
      <div className="app">
        <Header />
        <div className="container">
          <Routes>
            <Route path={homePage} element={<HomePage />} />
            <Route path={recipeListPage} element={<RecipesPage />} />
            <Route path="*" element={<Navigate to={homePage} replace />} />
          </Routes>
        </div>
        <Footer />
      </div>
    </BrowserRouter>
  );
}

export default App;

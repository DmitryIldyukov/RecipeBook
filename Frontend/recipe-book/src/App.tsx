import "./App.scss";
import { Footer } from "./components/footer/footer";
import { Header } from "./components/header/header";
import { HomePage } from "./components/homePage/homePage";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

function App() {
  return (
    <div className="app">
      <Header />
      <div className="container">
        <BrowserRouter>
          <Routes>
            <Route path="homePage" element={<HomePage />} />
            <Route path="*" element={<Navigate to="/homePage" replace />} />
          </Routes>
        </BrowserRouter>
      </div>
      <Footer />
    </div>
  );
}

export default App;

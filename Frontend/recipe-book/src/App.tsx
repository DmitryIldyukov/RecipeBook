import "./App.scss";
import { Header } from "./components/header/header";
import { HomePage } from "./components/homePage/homePage";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";

function App() {
  return (
    <div className="app">
      <div className="container">
        <BrowserRouter>
          <Header />
          <Routes>
            <Route path="homePage" element={<HomePage />} />
            <Route path="*" element={<Navigate to="/homePage" replace />} />
          </Routes>
        </BrowserRouter>
      </div>
    </div>
  );
}

export default App;

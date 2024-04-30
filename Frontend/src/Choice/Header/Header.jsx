import logo from "./invest.png";
import { Link } from "react-router-dom";
import "./Header.css";
import ButtonReg from "../Button/ButtonReg";

const Header = () => {
  return (
    <header>
      <Link to="/home" className="logo-button">
        <img src={logo} alt="Logo" className="logo" />
      </Link>
      <ButtonReg />
      <Link to="/bank" className="transparent-button">
        Интернет-банк
      </Link>
    </header>
  );
};

export default Header;

import "./Header.scss"
import Navigation from "Components/Navigation/Navigation"

const Header = ({ title, subtitle, imagePath, isNotLines }) => {
  return <header
    className="header"
    style={{ backgroundImage: `url("${imagePath}")` }}>
    <div className="header-container">
      <Navigation />
      <div className="title-block">
        <h1 className="title-block__title">{title}</h1>
        {!isNotLines && <div className="separator">
          <div className="separator__line-left" />
          <div className="separator__flower">✻</div>
          <div className="separator__line-right" />
        </div>}
        <p className="title-block__desc">{subtitle}</p>
      </div>
    </div>
  </header>
}

export default Header
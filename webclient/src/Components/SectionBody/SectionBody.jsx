import "./SectionBody.scss"

const SectionBody = ({ title, subtitle, imagePathes, imagePath }) => {
  return <section className="section">
    <div className="body-title">
      <h1 className="body-title__value">{title}</h1>
      <div className="body-title__flower">✻</div>
      <p className="body-title__subtitle">{subtitle}</p>
    </div>
    {imagePathes ? <div className="images">
      {imagePathes.map(path => <img
        className="images__img"
        src={path}
        alt="dont load" />
      )}
    </div> : <img
      className="section__img"
      src={imagePath}
      alt="dont load" />}
  </section >
}

export default SectionBody
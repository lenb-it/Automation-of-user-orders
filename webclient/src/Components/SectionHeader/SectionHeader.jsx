import "./SectionHeader.scss"

function SectionHeader({ title, imagePath }) {

	return <section
		className="section-header"
		style={{ backgroundImage: `url("${imagePath}")` }}>
		<div className="container">
			<h2 className="fs-2 fw-bold" style={{ letterSpacing: '8px' }}>{title}</h2>
		</div>
	</section>
}


export default SectionHeader;
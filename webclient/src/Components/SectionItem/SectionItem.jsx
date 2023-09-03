import "./SectionItem.css"

function SectionItem({ item }) {
	return <>
		<div className="flex-column section-item">
			<div>
				<img src={item.img} alt="" className="section-item-img" />
			</div>
			<div className="section-item-info">
				<div className="fst-italic fs-5 fw-bolder">
					{item.name}
				</div>
				<div>
					{item.description}
				</div>
				<div className="fw-bold">
					{item.price}
				</div>
			</div>
		</div>
	</>
}

export default SectionItem;
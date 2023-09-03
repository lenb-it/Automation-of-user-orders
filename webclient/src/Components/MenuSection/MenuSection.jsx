import SectionHeader from "Components/SectionHeader/SectionHeader"
import SectionItem from "Components/SectionItem/SectionItem"
import { Container } from "react-bootstrap";


function MenuSection({ title }) {
	const item = {
		img: "./img/1.jpg",
		name: "VODA",
		price: 20.49,
		description: "dsg dh sdhs hts hstrjhs a haehoj se jishasrhga oign aeuirgnuaei rf"
	}
	const item2 = {
		img: "./img/2.jpg",
		name: "VODA",
		price: 20.49,
		description: "dsg dh sd asg sdh sdhdsfh ssdh a asf asf asf asf asf as gad, kogsdojg osdngo dsjgdjdshsdh fagarg erhs hts hstrjhs a haehoj se jishasrhga oign aeuirgnuaei rf"
	}
	const item3 = {
		img: "./img/drink.jpg",
		name: "VODA",
		price: 20.49,
		description: "dsg dh sd  asf asf asf asf asf asg ag asg asdf sdfh sd dh sd gasasg sdh sdhdsfh ssdh a asf asf asf asf asf as gad, kogsdojg osdngo dsjgdjdshsdh fagarg erhs hts hstrjhs a haehoj se jishasrhga oign aeuirgnuaei rf"
	}
	return <>
		<div style={{ marginTop: "56px" }}></div>
		<SectionHeader title={title} imagePath={`./img/${title}.jpg`} />
		<Container>
			<div className="row justify-content-around align-items-stretch">
				{/* цикл */}
				<SectionItem item={item} />
				<SectionItem item={item3} />
				<SectionItem item={item2} />
				<SectionItem item={item} />
				<SectionItem item={item2} />
				<SectionItem item={item} />
			</div>
		</Container>
	</>
}

export default MenuSection;
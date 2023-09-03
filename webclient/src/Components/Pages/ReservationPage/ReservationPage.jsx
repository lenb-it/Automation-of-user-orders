import { reservation } from "Api/ApiRequest"
import "./ReservationPage.css"
import Header from "Components/Header/Header"
import { useContext, useState } from "react"
import { Form, Button } from "react-bootstrap"
import { observer } from "mobx-react-lite"
import { Context } from "index"
import ModalClose from "Components/ModalClose/ModalClose"

const ReservationPage = () => {
	const { user } = useContext(Context)
	const [date, setDate] = useState(new Date())
	const [time, setTime] = useState('10:00')
	const [modalShow, setModalShow] = useState(false);
	const [body, setBody] = useState('')
	const [countPeople, setCountPeople] = useState()

	const getCount = (count) => <option>{count}</option>

	function getTimes() {
		const times = []

		for (let i = 14; i <= 21; i += 1) {
			for (let j = 0; j <= 30; j += 30) {
				times.push({ hour: i, min: j + j === 0 ? '00' : j })
			}
		}

		return times
	}

	async function reservationPlace() {
		const newDate = `${date}T${time}:00.000Z`
		await reservation(localStorage.getItem("email"), newDate, countPeople).then(() => {
			setBody("Ваша заявка оставлена, ожидайте пока с вами свяжутся!")
			setModalShow(true)
		}).catch((e) => {
			setBody("Ошибка бронирования")
			setModalShow(true)
		})
	}

	function getTomorrowDate() {
		var date = new Date();
		date.setDate(date.getDate() + 1);
		let res = date.toLocaleDateString().split('.')
		return `${res[2]}-${res[1]}-${res[0]}`
	}

	return <>
		<Header
			title="Бронирование"
			subtitle="Забронировать столик онлайн несложно и занимает всего пару минут"
			imagePath="./img/contact-food.jpg" />
		<section className="section-reservation">
			<p className="section-reservation__title">Онлайн бронирование</p>
			<Form className="form">
				<Form.Group className="form-group">
					<img className="icon" src="./img/date.svg" alt="icon" />
					<Form.Control
						type="date"
						value={date}
						min={getTomorrowDate()}
						onChange={(e) => setDate(e.target.value)} />
				</Form.Group >
				<Form.Group className="form-group">
					<img className="icon" src="./img/time.svg" alt="icon" />
					<Form.Select onChange={(e) => setTime(e.target.value)}>
						{getTimes().map(time => <option>{time.hour}:{time.min}</option>)}
					</Form.Select>
				</Form.Group >
				<Form.Group className="form-group">
					<img className="icon" src="./img/people.svg" alt="icon" />
					<Form.Select onChange={(e) => setCountPeople(e.target.value)}>
						{new Array(10).fill(0).map((e, i) => getCount(i + 1))}
					</Form.Select>
				</Form.Group>
			</Form >
			<div className="reservation-submit">
				<Button variant="success" onClick={reservationPlace}>Забронировать</Button>
				<span style={{ fontStyle: 'italic', fontSize: '14px' }}>После отправки запроса с вами свяжутся для уточнения деталей</span>
			</div>
			<ModalClose
				show={modalShow}
				onHide={() => setModalShow(false)}
				body={body}
			/>
		</section >
	</>
}

export default observer(ReservationPage);
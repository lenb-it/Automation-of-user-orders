import Navigation from "Components/Navigation/Navigation";
import { Button, Form } from "react-bootstrap";
import "./RegistrationPage.css"
import ValidInput from "Components/ValidInput/ValidInput";
import FirstLastNameControl from "Components/FirstLastNameControl/FirstLastNameControl"
import { useContext, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Context } from "index";
import { observer } from "mobx-react-lite";
import { regin } from "Api/ApiRequest";
import ModalClose from "Components/ModalClose/ModalClose";

function RegistrationPage() {
	const { user } = useContext(Context)
	const [show, setShow] = useState(false);
	const history = useNavigate();

	async function submitRegin(e) {
		e.preventDefault()
		await regin(user.reginFields)
			.then(x => {
				user.setIsAuth(true);
				localStorage.setItem("token", x.data.token);
				localStorage.setItem("email", x.data.email);
				history("/");
			})
			.catch(x => {
				setShow(true)
			});
	}

	function onHide() {
		setShow(false)
	}

	return <>
		<Navigation />
		<ModalClose onHide={onHide} show={show} body={"Не получилось зарегистрироваться"} />
		<div className="back-grey">
			<div className="block-center">
				<Form>
					<Form.Group className="mb-3">
						<h3 className="text-center">Регистрация</h3>
					</Form.Group>
					<FirstLastNameControl />
					<ValidInput
						title="Email адрес"
						placeholder="Введите email"
						type="email" />
					<ValidInput
						title="Введите пароль"
						placeholder="Введите пароль"
						type="password" />
					<ValidInput
						title="Повторите пароль"
						placeholder="Введите пароль еще раз"
						type="password"
						isRepeat={true} />
					<ValidInput
						title="Номер телефона"
						placeholder="+375(xx)xxx-xx-xx"
						type="tel" />
					<Form.Group className="d-grid">
						<Button variant="primary" type="submit" onClick={(e) => submitRegin(e)}>Зарегистрироваться</Button>
					</Form.Group>
				</Form>
			</div>
		</div>
	</>
}

export default observer(RegistrationPage);
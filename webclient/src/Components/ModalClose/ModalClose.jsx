import { Modal } from "react-bootstrap";

export default function ModalClose({ show, onHide, body }) {
	return (
		<Modal
			show={show}
			onHide={onHide}
			size="lg"
			aria-labelledby="contained-modal-title-vcenter"
			centered
		>
			<Modal.Header closeButton>
				<Modal.Title id="contained-modal-title-vcenter">
					Уведомление
				</Modal.Title>
			</Modal.Header>
			<Modal.Body>
				<p>{body}</p>
			</Modal.Body>
		</Modal>
	);
}
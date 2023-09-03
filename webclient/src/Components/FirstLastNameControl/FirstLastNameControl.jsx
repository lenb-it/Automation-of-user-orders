import { Context } from "index"
import { observer } from "mobx-react-lite"
import { useState, useContext } from "react"
import { Form, InputGroup } from "react-bootstrap"

const ValidInput = observer(() => {
  const { user } = useContext(Context)
  const [error, setError] = useState(false)
  const [firstName, setFirstName] = useState('')
  const [lastName, setLastName] = useState('')

  const validation = () => {
    setError(false)

    firstName.trim() === '' && setError("Поле не должно быть пустым или состоять из одних пробелов")
    lastName.trim() === '' && setError("Поле не должно быть пустым или состоять из одних пробелов")
  }

  return <>
    <InputGroup>
      <InputGroup.Text>Имя и фамилия</InputGroup.Text>
      <Form.Control
        value={firstName}
        onChange={(e) => {
          setFirstName(e.target.value)
          user.setReginFields("firstName", e.target.value)
        }}
        aria-label="First name"
        onBlur={validation} />
      <Form.Control
        value={lastName}
        onChange={(e) => {
          setLastName(e.target.value)
          user.setReginFields("lastName", e.target.value)
        }}
        aria-label="Last name"
        onBlur={validation} />
    </InputGroup>
    {error &&
      <p className="text-danger text-wrap mb-3" style={{ fontSize: '14px' }}>{error}</p>}
  </>
})

export default ValidInput
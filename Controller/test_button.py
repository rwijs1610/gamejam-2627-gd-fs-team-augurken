import board
import digitalio
import time

button = digitalio.DigitalInOut(board.GP3)
button.switch_to_input(pull=digitalio.Pull.UP)

was_pressed = False

while True:
    pressed = not button.value  

    if pressed and not was_pressed:
        print("click!")

    was_pressed = pressed
    time.sleep(0.01)

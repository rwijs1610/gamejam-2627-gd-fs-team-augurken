from evdev import UInput, ecodes
from gpiozero import Button
import logging
from signal import pause

controls = {
    2: ecodes.KEY_W,
    3: ecodes.KEY_S,
    27: ecodes.KEY_A,
    22: ecodes.KEY_D,
    4: ecodes.KEY_1,
    17: ecodes.KEY_D,
    14: ecodes.KEY_C,
    15: ecodes.KEY_R,
    18: ecodes.KEY_LEFTSHIFT,
    23: ecodes.KEY_SPACE,
}

keyboard = UInput({ecodes.EV_KEY: list(controls.values())}, name="Arcade Controller")
buttons = []

logging.basicConfig(level=logging.INFO, format="%(message)s")

def press(pin, key):
    logging.info("GPIO %s pressed", pin)
    keyboard.write(ecodes.EV_KEY, key, 1)
    keyboard.syn()

def release(pin, key):
    logging.info("GPIO %s released", pin)
    keyboard.write(ecodes.EV_KEY, key, 0)
    keyboard.syn()

for pin, key in controls.items():
    button = Button(pin, pull_up=True, bounce_time=0.03)
    button.when_pressed = lambda pin=pin, key=key: press(pin, key)
    button.when_released = lambda pin=pin, key=key: release(pin, key)
    buttons.append(button)

try:
    pause()
except KeyboardInterrupt:
    pass
finally:
    keyboard.close()

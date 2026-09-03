from evdev import UInput, ecodes
from gpiozero import Button
from signal import pause

controls = {
    2: ecodes.KEY_W,
    3: ecodes.KEY_S,
    4: ecodes.KEY_A,
    27: ecodes.KEY_SPACE,
    14: ecodes.KEY_I,
    15: ecodes.KEY_K,
    18: ecodes.KEY_J,
    23: ecodes.KEY_L,
    22: ecodes.KEY_ENTER,
}

keyboard = UInput({ecodes.EV_KEY: list(controls.values())}, name="Arcade Controller")
buttons = []

for pin, key in controls.items():
    button = Button(pin, pull_up=True, bounce_time=0.03)
    button.when_pressed = lambda key=key: keyboard.write(ecodes.EV_KEY, key, 1)
    button.when_released = lambda key=key: keyboard.write(ecodes.EV_KEY, key, 0)
    buttons.append(button)

try:
    pause()
except KeyboardInterrupt:
    pass
finally:
    keyboard.close()

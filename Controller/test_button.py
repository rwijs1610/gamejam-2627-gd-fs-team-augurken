from gpiozero import Button

button = Button(3, pull_up=True)

print("Ready. Press the button connected between GPIO 3 and GND.")

while True:
    button.wait_for_press()
    print("Button clicked!")
    button.wait_for_release()

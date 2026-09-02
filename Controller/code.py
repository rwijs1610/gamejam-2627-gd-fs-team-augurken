import time
import board
import digitalio
import usb_hid

PINS = [board.GP2, board.GP3, board.GP4, board.GP5, board.GP6,
        board.GP7, board.GP8, board.GP9, board.GP10, board.GP11]

knoppen = []
for pin in PINS:
    knop = digitalio.DigitalInOut(pin)
    knop.switch_to_input(pull=digitalio.Pull.UP)
    knoppen.append(knop)

gamepad = usb_hid.devices[0]
report = bytearray(4)

while True:
    bits = 0
    for i in range(len(knoppen)):
        if not knoppen[i].value: 
            bits += 1 << i

    report[0] = bits & 255
    report[1] = bits >> 8
    gamepad.send_report(report)

    time.sleep(0.005)

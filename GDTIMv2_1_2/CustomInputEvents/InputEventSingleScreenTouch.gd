class_name InputEventSingleScreenTouch
extends InputEventAction

var position   : Vector2
var cancelled  : bool
var raw_gesture : RawGesture
var index : int

func _init(_index : int, _raw_gesture : RawGesture = null) -> void:
	index = _index
	raw_gesture = _raw_gesture
	if raw_gesture:
		pressed = raw_gesture.releases.empty()
		if pressed:
			position = raw_gesture.presses.values()[index].position
		else:
			position = raw_gesture.releases.values()[index].position
		cancelled = raw_gesture.size() > 1

func as_text() -> String:
	return "position=" + str(position) + "|pressed=" + str(pressed) + "|cancelled=" + str(cancelled)

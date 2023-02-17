class_name InputEventSingleScreenSwipe
extends InputEventAction

var position   : Vector2
var relative   : Vector2
var raw_gesture : RawGesture
var index : int

func _init(_index : int, _raw_gesture : RawGesture = null) -> void:
	index = _index
	raw_gesture = _raw_gesture
	if raw_gesture:
		position = raw_gesture.presses[0].position
		relative = raw_gesture.releases[0].position - position
	


func as_text() -> String:
	return "position=" + str(position) + "|relative=" + str(relative)

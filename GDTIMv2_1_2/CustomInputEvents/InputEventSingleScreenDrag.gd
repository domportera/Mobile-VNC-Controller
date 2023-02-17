class_name InputEventSingleScreenDrag
extends InputEventAction

var position   : Vector2
var relative   : Vector2
var raw_gesture : RawGesture
var index : int

func _init(_index : int, _raw_gesture : RawGesture = null) -> void:
	index = _index
	raw_gesture = _raw_gesture
	if raw_gesture:
		var dragEvent = raw_gesture.drags.values()[0]
		position = dragEvent.position
		relative = dragEvent.relative

func as_text():
	return "position=" + str(position) + "|relative=" + str(relative)

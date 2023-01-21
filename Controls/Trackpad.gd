extends Node

onready var trackpad_interpreter = get_node("../TrackpadInterpreter")

func _unhandled_input(event : InputEvent):

	if event is InputEventMultiScreenDrag:
		trackpad_interpreter.OnMultiDrag(event.position, event.relative, event.fingers)
		#var evt = event as InputEventMultiScreenDrag
	elif event is InputEventMultiScreenSwipe:
		trackpad_interpreter.OnMultiSwipe(event.position, event.relative, event.fingers)
	elif event is InputEventMultiScreenTap:
		trackpad_interpreter.OnMultiTap(event.position, event.fingers)
	elif event is InputEventMultiScreenLongPress:
		trackpad_interpreter.OnMultiLongPress(event.position, event.fingers)
	elif event is InputEventSingleScreenDrag:
		trackpad_interpreter.OnSingleDrag(event.position, event.relative)
	elif event is InputEventScreenPinch:
		trackpad_interpreter.OnPinch(event.position, event.relative, event.distance, event.fingers)
	elif event is InputEventScreenTwist:
		trackpad_interpreter.OnTwist(event.position, event.relative, event.fingers)
	elif event is InputEventSingleScreenTap:
		trackpad_interpreter.OnSingleTap(event.position)
	elif event is InputEventSingleScreenLongPress:
		trackpad_interpreter.OnSingleLongPress(event.position)
	elif event is InputEventSingleScreenTouch:
		trackpad_interpreter.OnSingleTouch(event.position, event.cancelled)
	elif event is InputEventSingleScreenSwipe:
		trackpad_interpreter.OnSingleSwipe(event.position, event.relative)
	elif event is RawGesture:
		pass
	
		
	# if event is RawGesture:
	# 	label3.text = "State now:\n" + event.as_text()
	# 	var rollback_res = event.rollback_relative(0.5)
	# 	var rollback_event = rollback_res[0]
	# 	var discarded_events = rollback_res[1]
	# 	label4.text = "State 0.5s ago:\n" + rollback_event.as_text()

	# 	var text = str(rollback_event.active_touches) + " |"
	# 	for e in discarded_events:
	# 		text += str(e.index)
	# 		if e is RawGesture.Drag:
	# 			text += "D "
	# 		else:
	# 			text += "T "
	# 	label5.text = "Events in the last 0.5s:\n"+text



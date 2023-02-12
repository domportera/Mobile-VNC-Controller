extends Control

enum EVENT_MODE { input, unhandled }
export(EVENT_MODE) var event_mode = EVENT_MODE.input

func _input(event : InputEvent):
	if event_mode == EVENT_MODE.input:
		_handle_input(event)

func _unhandled_input(event : InputEvent):
	if event_mode == EVENT_MODE.unhandled:
		_handle_input(event)

func _handle_input(event : InputEvent):	
	if event is InputEventMultiScreenDrag:
		GdtimForwarder.OnMultiDrag(event.position, event.relative, event.fingers)
	elif event is InputEventMultiScreenSwipe:
		GdtimForwarder.OnMultiSwipe(event.position, event.relative, event.fingers)
	elif event is InputEventMultiScreenTap:
		GdtimForwarder.OnMultiTap(event.position, event.fingers)
	elif event is InputEventMultiScreenLongPress:
		GdtimForwarder.OnMultiLongPress(event.position, event.fingers)
	elif event is InputEventSingleScreenDrag:
		GdtimForwarder.OnSingleDrag(event.position, event.relative)
	elif event is InputEventScreenPinch:
		GdtimForwarder.OnPinch(event.position, event.relative, event.distance, event.fingers)
	elif event is InputEventScreenTwist:
		GdtimForwarder.OnTwist(event.position, event.relative, event.fingers)
	elif event is InputEventSingleScreenTap:
		GdtimForwarder.OnSingleTap(event.position)
	elif event is InputEventSingleScreenLongPress:
		GdtimForwarder.OnSingleLongPress(event.position)
	elif event is InputEventSingleScreenTouch:
		GdtimForwarder.OnSingleTouch(event.position, event.pressed, event.cancelled, event.raw_gesture)
	elif event is InputEventSingleScreenSwipe:
		GdtimForwarder.OnSingleSwipe(event.position, event.relative)
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
